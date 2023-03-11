import { Router } from '@angular/router';
import { UserSession } from './user-session';
import { BaseResponse } from './../../core/base/base-result';
import { Login } from './login';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { BaseService } from 'src/app/core/base/base.service';
import { map, catchError } from 'rxjs/operators';
import { Buffer } from 'buffer';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseService {
  userSession: UserSession;

  constructor(
    private httpClient: HttpClient,
    private route: Router) {
    super();
  }

  authentication(login: Login): Observable<UserSession>{
    let uri = `${this.baseUrl}/authenticated/login`;
    return this.httpClient.post<BaseResponse<UserSession>>(uri, login)
      .pipe(map(this.validationResult), catchError(this.errorHandler));
  }

  authenticationWithGoogle(login: string) : Observable<UserSession>{
    const header = new HttpHeaders().set('Content-type', 'application/json');
    let uri = `${this.baseUrl}/authenticated/loginwithgoogle`;
    return this.httpClient.post<BaseResponse<UserSession>>(uri, JSON.stringify(login), { headers: header })
      .pipe(map(this.validationResult), catchError(this.errorHandler));
  }

  get logado(): boolean {
    var accessToken = localStorage.getItem('access_token');
    if (!accessToken) return false;
    this.userSession = Object.assign({}, this.userSession, accessToken);

    let currentDate = new Date();
    if (currentDate > this.userSession.expireTymeSpan) return false;
    return true;
  }

  get getToken(): string | null {
    var accessToken = localStorage.getItem('access_token');
    if (!accessToken) return null;

    var accessTokenDecode = Buffer.from(accessToken, 'base64').toString('binary');
    this.userSession = Object.assign({}, this.userSession, accessTokenDecode);
    return this.userSession.token;
  }

  logout() {
    localStorage.clear();
    this.route.navigate(['login']).then(() => window.location.reload());;
  }
}
