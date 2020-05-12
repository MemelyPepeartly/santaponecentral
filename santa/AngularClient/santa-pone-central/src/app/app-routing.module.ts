import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './home/login/login.component';
import { SignupComponent } from './signup/signup.component';
import { AdminnavComponent } from './navbar/adminnav/adminnav.component';
import { ContactComponent } from './contact/contact.component';
import { HeadquartersComponent } from './headquarters/headquarters.component';
import { IncomingCorrespondenceComponent } from './headquarters/incoming-correspondence/incoming-correspondence.component';
import { AdminHelpComponent } from './admin-help/admin-help.component';
import { AdminControlComponent } from './admin-control/admin-control.component';

const appRoutes: Routes = [
  { path: 'profile',
    component: ProfileComponent
    
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'santa-central',
    component: AdminnavComponent
  },
  {
    path: 'signup',
    component: SignupComponent
  },
  {
    path: 'contact',
    component: ContactComponent
  },
  {
    path: 'headquarters',
    component: HeadquartersComponent
  },
  {
    path: 'correspondence',
    component: IncomingCorrespondenceComponent
  },
  {
    path: 'help',
    component: AdminHelpComponent
  },
  {
    path: 'admin-control',
    component: AdminControlComponent
  },
  {
		path: "**",
		redirectTo: "/home"
  },
  { 
    path: '',
    redirectTo: '/home',
    pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
