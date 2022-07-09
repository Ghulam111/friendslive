import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountsService } from './../_services/accounts.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister = new EventEmitter();
  registerForm : FormGroup;
  maxdate : Date;
  validationErrors : string[] = [];

  constructor(private accountservice: AccountsService,private toastr: ToastrService, private fb : FormBuilder, private router : Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxdate = new Date();
    this.maxdate.setFullYear(this.maxdate.getFullYear() - 18);
  }

  initializeForm()
  {
    this.registerForm = this.fb.group({
      gender : ['male'],
      knownAs : ['',Validators.required],
      dateOfBirth : ['',Validators.required],
      city : ['',Validators.required],
      country : ['',Validators.required],
      username : ['',Validators.required],
      password : ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmpassword: ['',[Validators.required,this.MatchValues('password')]]
    })
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmpassword.updateValueAndValidity();
    })
  }

  MatchValues(matchto:string): ValidatorFn 
  {
    return(control : AbstractControl) => {
      return control?.value === control?.parent?.controls[matchto].value ? null : {Ismatching : true}
    }
  }

  register()
  {
    this.accountservice.register(this.registerForm.value).subscribe(response =>{
      this.router.navigateByUrl('/members');
    },error =>{
     this.validationErrors = error;
      this.toastr.error(error.error);
    })
    
  }

  cancel(){
    this.cancelRegister.emit(false);
  }


}
