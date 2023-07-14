import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment, TokenResponse } from '../model';
import { TokenService } from './token.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(public http: HttpClient, private router: Router) { }


  postData<TResponse, TBody>(
    url: string,
    body: TBody,
    options: {
      headers?: { [header: string]: string },
      params?: { [params: string]: string }
    } = {}
  ): Observable<TResponse> {

   


    return this.http.post<TResponse>(`${environment.apiUrl}/api/${url}`, body, { headers: options.headers, params: options.params });
  }

  getData<TResponse>(
    url: string,
    options: {
      headers?: { [header: string]: string },
      params?: { [params: string]: string }
    } = {}
  ): Observable<TResponse> {
    
    return this.http.get<TResponse>(`${environment.apiUrl}/api/${url}`, { headers: options.headers, params: options.params });
  }

  getBlobData(
    url: string,
    params?: {
      [params: string]: any
    },
  ) {
    return this.http.get(`${environment.apiUrl}/api/${url}`, {
      headers: { 'Content-Type': 'application/json' },
      responseType: 'blob', params
    });
  }
}
