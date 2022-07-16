import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static:true}) memberTabs : TabsetComponent
  member : Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
 

constructor(private member_service: MembersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.subscribe(data =>{
      this.member = data.member;
    })


    this.route.queryParams.subscribe(params =>{
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })


    this.galleryOptions = [
      {
        width : '500px',
        height : '500px',
        imagePercent : 100,
        thumbnailsColumns: 4,
        imageAnimation  : NgxGalleryAnimation.Slide,
        preview : false
      }
    ]

    this.galleryImages = this.getphotos();
   
  }
  
  getphotos() : NgxGalleryImage[] {
    const imageUrls = [];
    for (let photo of this.member.photos){
      imageUrls.push({
        small : photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    }
    return imageUrls;
  }

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }

  loadMember(){
    this.member_service.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member =>{
      this.member = member;
      
    })
     
  }
}
