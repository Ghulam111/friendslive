
import { AccountsService } from './../_services/accounts.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any = {};
  Isloggedin: boolean;


  constructor(public accountservice: AccountsService) { }

  ngOnInit(): void {
    
  }

  login() {
    this.accountservice.login(this.model).subscribe(response => {
      console.log(response);
     

    }, error => {
      console.log(error);
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
   
  }

}
