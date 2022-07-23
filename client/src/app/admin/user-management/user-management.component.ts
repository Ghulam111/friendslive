import { User } from 'src/app/_models/User';
import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
users: Partial<User[]>;
bsModalRef : BsModalRef;

  constructor(private adminService : AdminService, private modalService : BsModalService) { }

  ngOnInit(): void {
    this.loadUserswithRoles();
  }

  loadUserswithRoles()
  {
    this.adminService.getUsersWithroles().subscribe(response => {
      this.users = response;
    });
  }

  OpenRolesModal(user : User)
  {
    const config={
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roles : this.getRolesArray(user)
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.content.updateSelectedRoles.subscribe( values =>{
      const rolestoUpdate = {
        roles : [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if(rolestoUpdate)
      {
        this.adminService.updateRoles(user.username, rolestoUpdate.roles).subscribe(() =>{
          user.roles = [...rolestoUpdate.roles]
        })
      }
    })
   
  }

  private getRolesArray(user : User) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles : any[] = [
      {name : 'Admin', value : 'Admin'},
      {name : 'Member', value : 'Member'},
      {name : 'Moderator', value : 'Moderator'}
    ]
    availableRoles.forEach( role => {
      let match =  false;
      for(const userole of userRoles){
        if(userole === role.name){
          match = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if(!match){
        role.checked = false;
        roles.push(role);
      }
    })
    return roles;
  }


  }


