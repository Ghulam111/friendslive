import { PaginatedResult, Pagination } from './../_models/pagination';
import { MembersService } from 'src/app/_services/members.service';
import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/Member';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members : Partial<Member[]>;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 3;
  pagination : Pagination;

  constructor(private memberservice : MembersService) { }

  ngOnInit(): void {
    this.loadlikedUsers();
    console.log(this.members);
  }

  loadlikedUsers()
  {
    this.memberservice.getLikes(this.predicate, this.pageNumber,this.pageSize).subscribe(response => {
      console.log(response);
      this.members = response.result;
      this.pagination = response.pagination;
    })

  }

  pageChanged(event:any){
    this.pageNumber = event.page;
    this.loadlikedUsers();
  }

}
