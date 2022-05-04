import { AccountsService } from './../_services/accounts.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  
  @Output() cancelRegister = new EventEmitter();

  constructor(private accountservice: AccountsService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountservice.register(this.model).subscribe(response =>{
      console.log(response);
      this.cancel();
    },error =>{
      console.log(error);
    })
    
  }

  cancel(){
    this.cancelRegister.emit(false);
  }


}
