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
  { path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
