import { take } from 'rxjs/operators';
import { AccountsService } from './../../_services/accounts.service';
import { UserParams } from './../../_models/UserParams';
import { Pagination } from './../../_models/pagination';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members : Member[];
  pagination : Pagination;
  userParams : UserParams;
  user : User;
  genderList = [{value:'male',display:'Males'},{value:'female',display:'Females'}];
  
  constructor(private memberservice : MembersService, private account_service : AccountsService) {
    this.account_service.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    })
   }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(){
    this.memberservice.getMembers(this.userParams).subscribe(response=>{

      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  resetfilters(){
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

}
