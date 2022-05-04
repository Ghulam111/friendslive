import { ToastrService } from 'ngx-toastr';
import { AccountsService } from './../_services/accounts.service';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators'
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

constructor(private accountsservice: AccountsService, private toastr : ToastrService)
{

}
  canActivate(): Observable<boolean> {
    return this.accountsservice.currentUser$.pipe(
      map(user =>{
        if (user) return true;
        this.toastr.error("you shall not pass !");
      })
    )
    
  }
  
}
