import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, throwError } from 'rxjs';
import { mergeMap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InterceptorService implements HttpInterceptor {

  constructor(private auth: AuthService) { }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log(this.auth.isAuthenticated$);
    
    if(this.auth.isAuthenticated$)
    {
      return this.auth.idTokenClaims$.pipe(
        mergeMap(token => {
          console.log("---Token info---");
          console.log(token);
          
          const tokenReq = req.clone({
            setHeaders: { Authorization: `Bearer ${token.__raw}` }
          });
          return next.handle(tokenReq);
        }),
        catchError(err => throwError(err))
      );
    }
    else
    {
      return next.handle(req);
    }
  }
}