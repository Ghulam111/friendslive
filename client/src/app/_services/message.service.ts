import { Message } from './../_models/Messages';
import { Member } from './../_models/Member';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getPaginatedResults, getPaginationHeaders } from './paginationHelper';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseurl  = environment.apiUrl;

  constructor(private http: HttpClient) 
  {

   }
   getMessages(pageNumber,pageSize,container:string)
   {
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('Container',container);
    return getPaginatedResults<Message[]>(this.baseurl + 'messages', params, this.http);
   }

   GetMessageThread(username : string)
   {
    return this.http.get<Message[]>(this.baseurl + 'messages/thread/'+ username);
    
   }

   SendMessage(username : string, content : string)
   {
    return this.http.post<Message>(this.baseurl + 'messages',{recipientUsername : username,Content : content})
   }

   deletemessage(id: number)
   {
    return this.http.delete(this.baseurl + 'messages/'+ id);
   }

}
