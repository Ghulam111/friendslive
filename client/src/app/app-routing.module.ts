import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { AuthGuard } from './_guards/auth.guard';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes, CanActivate } from '@angular/router';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberEditComponent } from './members/member-edit/member-edit/member-edit.component';

const routes: Routes = [
  {path:'' , component:HomeComponent},
  {path:'',
  runGuardsAndResolvers:'always',
  children:[
    {path:'members',component:MemberListComponent},
    {path:'members/:username',component:MemberDetailComponent },
    {path:'member/edit',component:MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard] },
    {path:'lists',component:ListsComponent},
    {path:'messages', component:MessagesComponent},
  ],
  canActivate: [AuthGuard]
  },
  {path:'**',component:HomeComponent, pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
