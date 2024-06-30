import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginResDto, LoginReqDto, RefreshTokenReqDto, RegisterReqDto } from '../models/account.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AccountApiService {
  constructor(private http: HttpClient) { }

  login(loginReqDto:LoginReqDto): Observable<LoginResDto> {
    const url = this.getUrl(AccountUrls.Login);
    return this.http.post<LoginResDto>(url, loginReqDto);
  }

  register(registerReqDto: RegisterReqDto): Observable<number> {
    const url = this.getUrl(AccountUrls.Register);
    return this.http.post<number>(url, registerReqDto);
  }

  refreshToken(refreshTokenReqDto: RefreshTokenReqDto): Observable<LoginResDto> {
    const url = this.getUrl(AccountUrls.RefreshToken);
    return this.http.post<LoginResDto>(url, refreshTokenReqDto);
  }

  revokeToken(refreshTokenReqDto: RefreshTokenReqDto): Observable<void> {
    const url = this.getUrl(AccountUrls.RevokeToken);
    return this.http.post<void>(url, refreshTokenReqDto);
  }

  private getUrl(accountUrl: AccountUrls) {
     return `${environment.api}/api/identity/${accountUrl}`;
  }
}

enum AccountUrls {
  Register = 'register',
  Login = 'login',
  RefreshToken = 'refreshToken',
  RevokeToken = 'revokeToken',
}