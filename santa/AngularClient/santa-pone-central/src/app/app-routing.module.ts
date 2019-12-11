import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './splash/splash.component';
import { ProfileComponent } from './profile/profile.component';
import { SantaponecontrolComponent } from './santaponecontrol/santaponecontrol.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SantaponenavComponent } from './navbar/santaponenav/santaponenav.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './splash/login/login.component';
import { SignupComponent } from './signup/signup.component';

const appRoutes: Routes = [
  { path: 'profile',
    component: ProfileComponent
  },
  {
    path: 'splash',
    component: HomeComponent
  },
  {
    path: 'santa-control',
    component: SantaponecontrolComponent
  },
  {
    path: 'signup',
    component: SignupComponent
  },
  {
    path: 'contact',
    component: SantaponecontrolComponent
  },
  { path: '',
    redirectTo: '/splash',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
