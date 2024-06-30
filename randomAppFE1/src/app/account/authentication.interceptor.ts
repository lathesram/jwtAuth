import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
} from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  Subject,
  catchError,
  filter,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { Router } from '@angular/router';
import { TokenService } from './services/token.service';
import { AccountService } from './services/account.service';
import { LoggedInUser } from './models/account.model';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  private isRefreshTokenUpdating = false;
  private refreshTokenSubject: Subject<LoggedInUser | undefined> =
    new BehaviorSubject(this.tokenService.getLoggedInUser());

  constructor(
    private router: Router,
    private accountService: AccountService,
    private tokenService: TokenService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.tokenService.getAccessToken() || '';

    if (this.isValidToken(accessToken)) {
      req = this.setHeaders(accessToken, req);
    }

    return next.handle(req).pipe(
      catchError((err) => {
        if (err.status === 401 && !req.url.includes('refreshToken')) {
          return this.handleUnauthorizedError(req, next);
        } else {
          return throwError(() => new Error(err));
        }
      })
    );
  }

  private isValidToken(accessToken: string) {
    return accessToken && accessToken?.length > 0;
  }

  private setHeaders(
    accessToken: string,
    req: HttpRequest<any>
  ): HttpRequest<any> {
    req = req.clone({
      setHeaders: {
        'Content-Type': 'application/json; charset=utf-8',
        Accept: 'application/json',
        Authorization: `Bearer ${accessToken}`,
      },
    });
    return req;
  }

  private handleUnauthorizedError(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.isRefreshTokenUpdating) {
      this.isRefreshTokenUpdating = true;
      this.refreshTokenSubject.next(undefined);

      return this.accountService.refreshToken().pipe(
        switchMap((credentials: LoggedInUser) => {
          this.isRefreshTokenUpdating = false;
          this.refreshTokenSubject.next(credentials);
          return next.handle(this.setHeaders(credentials.accessToken, req));
        }),
        catchError((err) => {
          this.isRefreshTokenUpdating = false;
          this.router.navigateByUrl('login');
          return throwError(() => new Error(err));
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter((loggedInUser) => loggedInUser !== undefined),
        take(1),
        switchMap((loggedInUser) => {
          return next.handle(this.setHeaders(loggedInUser!.accessToken, req));
        })
      );
    }
  }
}
