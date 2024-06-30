import { LoggedInUser } from './../models/account.model';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private localStorageKey = 'LoggedInUser';

  setLoggedInUser(loggedInUser?: LoggedInUser) {
    localStorage.setItem(this.localStorageKey, JSON.stringify(loggedInUser));
  }

  getLoggedInUser(): LoggedInUser | undefined {
    const loggedInUser = localStorage.getItem(this.localStorageKey);
    return loggedInUser ? JSON.parse(loggedInUser) : undefined;
  }  
  
  removeLoggedInUser() {
    localStorage.removeItem(this.localStorageKey);
  }

  getAccessToken(): string | undefined {
    return this.getLoggedInUser()?.accessToken;
  }

  getRefreshToken(): string | undefined {
    return this.getLoggedInUser()?.refreshToken;
  }

  getAccessTokenExpiration(): string | undefined {
    return this.getLoggedInUser()?.accessTokenExpiration;
  }
}
