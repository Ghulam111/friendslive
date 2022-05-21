import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { AccountsService } from 'src/app/_services/accounts.service';
import { MembersService } from 'src/app/_services/members.service';
import { Member } from 'src/app/_models/Member';
import { User } from 'src/app/_models/User';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
@ViewChild('editform') editform: NgForm;
  member : Member;
  user : User;
  @HostListener('window:beforeunload',['$event']) unloadNotification($event: any){

    if(this.editform.dirty){
      $event.returnValue = true;
    }

  }

  constructor(private account_servive: AccountsService, private member_service: MembersService, private toastr: ToastrService) { 
    this.account_servive.currentUser$.pipe(take(1)).subscribe(user =>{ this.user = user});
  }

  ngOnInit(): void {
    this.loadMember();
  }

loadMember(){
  this.member_service.getMember(this.user.username).subscribe(member =>{ this.member = member});
}

UpdateMember(){
  
  this.member_service.updateMember(this.member).subscribe(() => {
    this.toastr.success('Profile updated successfully');
    this.editform.reset(this.member);
  })
  
}

}
