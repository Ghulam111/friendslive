import { Photo } from './../../_models/Photo';
import { take } from 'rxjs/operators';
import { environment } from './../../../environments/environment';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Member } from 'src/app/_models/Member';
import { AccountsService } from 'src/app/_services/accounts.service';
import { User } from 'src/app/_models/User';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() member : Member;
  uploader : FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user : User;

  constructor(private accountservice : AccountsService,private memberservice : MembersService) {
    this.accountservice.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
   }

  
  ngOnInit(): void {
    this.initializeUploader();
  }
  fileOverBase(e : any){
  this.hasBaseDropZoneOver = e;
}

  initializeUploader()
  {
    this.uploader = new FileUploader({
      url : this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5 : true,
      allowedFileType : ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item,response,status,headers) =>{
      if(response){
        const photo : Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if(photo.isMain){
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountservice.setCurrentUser(this.user);
        }
      }
    }
  }

  setmainPhoto(photo : Photo)
  {
    this.memberservice.setMainPhoto(photo.id).subscribe(() => {
      this.user.photoUrl = photo.url;
      this.accountservice.setCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p => {
        if(p.isMain) p.isMain = false;
        if(p.id === photo.id) p.isMain = true;
      })
    })
  }

  Deletephoto(photoId : number)
  {
    this.memberservice.deletePhoto(photoId).subscribe(()=> {
      this.member.photos = this.member.photos.filter(x => x.id !== photoId);
      
    })
  }
}
