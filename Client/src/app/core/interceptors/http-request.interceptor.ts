import { AuthenticationService } from './../../client/login/authentication.service';
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
    constructor(private authenticationService : AuthenticationService) {}
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> | Observable<never> {
        const token = this.authenticationService.getToken;

        if (token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`,
                    token: `${token}`
                }
            });
            return next.handle(request).pipe(catchError(this.errorHandler));
        }
        else {
            return next.handle(request);
        }
    }

    private errorHandler(error: HttpErrorResponse): Observable<never> {
      if (error.status == HttpStatusCode.Unauthorized)
        this.authenticationService.logout();

      throw new Error(error.message);
    }
}
