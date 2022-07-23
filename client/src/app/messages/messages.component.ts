import { Pagination, PaginatedResult } from './../_models/pagination';
import { MessageService } from './../_services/message.service';
import { Message } from './../_models/Messages';
import { Component, OnInit } from '@angular/core';
import { ConfirmService } from '../_services/confirm.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pageSize = 5;
  pageNumber = 1;
  container = 'Unread';
  pagination : Pagination;

  constructor(private messageService : MessageService, private confirmService : ConfirmService)
  { 

  }

  ngOnInit(): void 
  {
    this.loadMessages();
  }

  loadMessages()
  {
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container)
    .subscribe(response =>{
      this.messages = response.result,
      this.pagination = response.pagination
    })
  }

  deleteMessage(id: number)
  {
    this.confirmService.confirm("Confirm delete message","Are you sure you want to delete this message ?").subscribe(result =>{
      if(result ){
        this.messageService.deletemessage(id).subscribe(()=>{
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
      })
      }
    })
  }

  pageChanged(event:any)
  {
    if(this.pageNumber !== event.page)
    {
      this.pageNumber = event.page;
      this.loadMessages();
    }
    
  }

}
