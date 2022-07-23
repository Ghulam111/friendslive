import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountsService } from '../_services/accounts.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private accountService : AccountsService, private toastr: ToastrService) {}


  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(map( user => {
      if(user.roles.includes("Admin") || user.roles.includes("Moderator")){
        return true;
      }
      this.toastr.error("You are not authoized to use this page");
    }))
  }
  
}
