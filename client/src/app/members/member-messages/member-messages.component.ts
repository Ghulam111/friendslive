import { NgForm } from '@angular/forms';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { throwError } from 'rxjs';
import { Message } from 'src/app/_models/Messages';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() username: string;
   messages : Message[];
  messageContent : string;

  constructor(private messageService : MessageService) { }

  ngOnInit(): void {
  this.loadMessages();
  }

  sendMessage()
  {
    this.messageService.SendMessage(this.username,this.messageContent).subscribe(message =>{
      this.messages.push(message);
      this.messageForm.reset();
    })
  }

  loadMessages()
  {
    this.messageService.GetMessageThread(this.username).subscribe(response => {
      this.messages = response;
    })

  }

}
