import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountsService } from '../_services/accounts.service';
import { take } from 'rxjs/operators';
import { User } from '../_models/User';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountsService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser : User;

    this.accountService.currentUser$.pipe(take(1)).subscribe(user=> currentUser = user);
    
    if(currentUser){
      request = request.clone({
        setHeaders:{
          Authorization : `Bearer ${currentUser.token}`
        }
      })
    }
    return next.handle(request);
  }
}
