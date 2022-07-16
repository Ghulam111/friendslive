import { UserParams } from './../_models/UserParams';
import { Member } from './../_models/Member';
import { PaginatedResult } from './../_models/pagination';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Injectable } from '@angular/core';

import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { JsonPipe } from '@angular/common';
import { getPaginatedResults, getPaginationHeaders } from './paginationHelper';



@Injectable({
  providedIn: 'root'
})


export class MembersService {
  baseurl  = environment.apiUrl;
  members: Member[] = [];
 

  constructor(private http : HttpClient) { }

  getMembers(userParams : UserParams)
  {
    
     let params =  getPaginationHeaders(userParams.pageNumber,userParams.pageSize);

     params = params.append('minAge',userParams.minAge.toString());
     params = params.append('maxAge',userParams.maxAge.toString());
     params = params.append('gender',userParams.gender);
     params = params.append('orderBy',userParams.orderBy);
   
    return getPaginatedResults<Member[]>(this.baseurl + 'users', params,this.http);
    
    
  }

  addLike(username: string)
  {
    return this.http.post(this.baseurl + 'likes/'+ username,{});
  }

  getLikes(predicate: string, pageNumber, pageSize)
  {
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('predicate',predicate);

    return getPaginatedResults<Partial<Member[]>>(this.baseurl + 'likes',params, this.http); 
   
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
