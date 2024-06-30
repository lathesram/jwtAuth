import { TokenService } from './token.service';
import { RefreshTokenReqDto, LoggedInUser } from './../models/account.model';
import { AccountApiService } from './account.api.service';
import { Injectable } from '@angular/core';
import { LoginReqDto, RegisterReqDto } from '../models/account.model';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(
    private accountApiService: AccountApiService,
    private tokenService: TokenService
  ) {}

  login(loginReqDto: LoginReqDto) {
    this.accountApiService
      .login(loginReqDto)
      .pipe(
        tap((loggedInUser: LoggedInUser) => {
          if (loggedInUser) this.tokenService.setLoggedInUser(loggedInUser);
        })
      )
      .subscribe((res) => {
        if (!res) return;
        console.log('LoggedIn!');
      });
  }

  logout() {
    this.tokenService.removeLoggedInUser();
  }

  register(registerReqDto: RegisterReqDto) {
    this.accountApiService.register(registerReqDto).subscribe((res) => {
      console.log('Success!');
    });
  }

  refreshToken(): Observable<LoggedInUser> {
    const refreshTokenReqDto: RefreshTokenReqDto = {
      refreshTokenString: this.tokenService.getRefreshToken() || '',
    };
    
    return this.accountApiService.refreshToken(refreshTokenReqDto).pipe(
      tap((loggedInUser) => {
        this.tokenService.setLoggedInUser(loggedInUser);
      })
    );
  }

  revokeToken(refreshTokenReqDto: RefreshTokenReqDto) {
    this.accountApiService.revokeToken(refreshTokenReqDto).subscribe((res) => {
      console.log('Refresh Token Revoked');
    });
  }
}
