import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SplashComponent } from './splash/splash.component';
import { ProfileComponent } from './profile/profile.component';
import { SantaponecontrolComponent } from './santaponecontrol/santaponecontrol.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SantaponenavComponent } from './navbar/santaponenav/santaponenav.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './splash/login/login.component';
import { SignupComponent } from './splash/signup/signup.component';
import { GeneralInformationComponent } from './splash/general-information/general-information.component';

const appRoutes: Routes = [
  { path: 'profile',
    component: ProfileComponent
  },
  {
    path: 'splash',
    component: SplashComponent
  },
  {
    path: 'santa-control',
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
