import { UserParams } from './../_models/UserParams';
import { Member } from './../_models/Member';
import { PaginatedResult } from './../_models/pagination';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Injectable } from '@angular/core';

import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { JsonPipe } from '@angular/common';



@Injectable({
  providedIn: 'root'
})


export class MembersService {
  baseurl  = environment.apiUrl;
  members: Member[] = [];
 

  constructor(private http : HttpClient) { }

  getMembers(userParams : UserParams)
  {
    
     let params =  this.getPaginationHeaders(userParams.pageNumber,userParams.pageSize);

     params = params.append('minAge',userParams.minAge.toString());
     params = params.append('maxAge',userParams.maxAge.toString());
     params = params.append('gender',userParams.gender);
     params = params.append('orderBy',userParams.orderBy);
   
    return this.getPaginatedResults<Member[]>(this.baseurl + 'users', params);
    
    
  }

  private getPaginatedResults<T>(url,params) {
   const paginatedResult : PaginatedResult<T> = new  PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumebr : number, pageSize: number)
  {
    var params = new HttpParams();

    params = params.append('pageNumber',pageNumebr.toString());
    params = params.append('pageSize',pageSize.toString());
    
    return params;
  }

  getMember(username : string)
  {
    const member = this.members.find(u => u.userName === username);
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseurl + 'users/' + username);
  }

  updateMember(member : Member){
    return this.http.put(this.baseurl + 'users', member).pipe(
      map(()=> {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }
  deletePhoto(photoId : Number)
  {
    return this.http.delete(this.baseurl + 'users/delete-photo/'+ photoId);
  }
  setMainPhoto(photoId: Number)
  {
    return this.http.put(this.baseurl + 'users/set-main-photo/'+ photoId,{});
  }
}
