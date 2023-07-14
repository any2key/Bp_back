import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { TokenResponse } from '../model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private userService: UserService, private router: Router) { }

  saveSession(tokenResponse: TokenResponse) {

    window.localStorage.setItem('AT', tokenResponse.accessToken);
    window.localStorage.setItem('RT', tokenResponse.refreshToken);
    if (tokenResponse.userId) {
      window.localStorage.setItem('ID', tokenResponse.userId.toString());
      window.localStorage.setItem('LN', tokenResponse.login);
      window.localStorage.setItem('RL', tokenResponse.userRole);
    }

  }

  getSession(): TokenResponse | null {
    if (window.localStorage.getItem('AT')) {
      const tokenResponse: TokenResponse = {
        accessToken: window.localStorage.getItem('AT') || '',
        refreshToken: window.localStorage.getItem('RT') || '',
        login: window.localStorage.getItem('LN') || '',
        userRole: (window.localStorage.getItem('RL') || ''),
        userId: (window.localStorage.getItem('ID') || '0')
      };

      return tokenResponse;
    }
    return null;
  }

  logout() {
    window.localStorage.clear();
    this.router.navigate(['login']);
  }

  isLoggedIn(): boolean {
    let session = this.getSession();
    if (!session) {
      return false;
    }

    // check if token is expired
    const jwtToken = JSON.parse(atob(session.accessToken.split('.')[1]));
    const tokenExpired = Date.now() > (jwtToken.exp * 1000);

    return !tokenExpired;

  }

  refreshToken(session: TokenResponse) {
    return this.userService.refreshToken(session).subscribe(res => {
      this.saveSession(res.data);
    });
  }

  checkToken() {
    let t = this.getSession();
    if (t != null) {
      const jwtToken = JSON.parse(atob(t!.accessToken.split('.')[1]));
      const tokenExpired = Date.now() > (jwtToken.exp * 1000);
      if (tokenExpired) {
        this.refreshToken(t);
      }
    }
  }

}
