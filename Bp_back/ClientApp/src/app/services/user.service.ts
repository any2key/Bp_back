import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { APIResponse, DataResponse, LoginRequest, SignupRequest, TokenResponse } from '../model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api: ApiService) { }

  login(loginRequest: LoginRequest): Observable<DataResponse<TokenResponse>> {
    return this.api.postData<DataResponse<TokenResponse>, LoginRequest>(`user/login`, loginRequest);
  }

  signup(SignupRequest: SignupRequest) {
    return this.api.postData < APIResponse, SignupRequest>(`user/signup`, SignupRequest,);
  }

  refreshToken(session: TokenResponse) {
    let refreshTokenRequest: any = {
      UserId: session.userId,
      RefreshToken: session.refreshToken
    };
    return this.api.postData<DataResponse<TokenResponse>, any>(`user/refresh_token`, refreshTokenRequest);
  }

  logout() {
    return this.api.postData < APIResponse, null>(`user/signup`, null);
  }

  //getUserInfo(): Observable<UserResponse> {
  //  return this.httpClient.get<UserResponse>(`${environment.apiUrl}/users/info`);
  //}

}
