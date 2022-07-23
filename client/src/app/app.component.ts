import { User } from './_models/User';
import { AccountsService } from './_services/accounts.service';


import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The friends App';
  

  constructor( private accountservice: AccountsService){ }

  ngOnInit() {
   this.setCurrentUser();
  }

  setCurrentUser(){
    const user : User = JSON.parse(localStorage.getItem('user'));
    this.accountservice.setCurrentUser(user);
  }
 
}

