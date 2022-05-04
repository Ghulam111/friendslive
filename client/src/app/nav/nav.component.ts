import { ToastrModule, ToastrService } from 'ngx-toastr';

import { AccountsService } from './../_services/accounts.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {};
  Isloggedin: boolean;


  constructor(public accountservice: AccountsService,private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

  login() {
    this.accountservice.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
      this.toastr.success("logged in successfully");

    }, error => {
      console.log(error);
      this.toastr.error(error.error);
      
    })
    
  }

  // getCurrentUser(){
  //   this.accountservice.currentUser$.subscribe(user => {
  //     this.Isloggedin = !!user;
  //   }, error =>{
  //     console.log(error);
  //   })
  // }

  logout(){
    this.accountservice.logout();
    this.router.navigateByUrl('/')
   
  }

}
