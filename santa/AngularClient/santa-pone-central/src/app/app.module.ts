//Necessary
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

//Extras
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CustomMaterialModule } from './core/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

//Componenents
import { SplashComponent } from './splash/splash.component';
import { ProfileComponent } from './profile/profile.component';
import { SantaponecontrolComponent } from './santaponecontrol/santaponecontrol.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SantaponenavComponent } from './navbar/santaponenav/santaponenav.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './splash/login/login.component';
import { SignupComponent } from './splash/signup/signup.component';
import { InformationComponent } from './splash/information/information.component';


const appRoutes: Routes = [
  { path: 'profile', component: ProfileComponent },
  {
    path: 'splash',
    component: SplashComponent
  },
  { path: '',
    redirectTo: '/splash',
    pathMatch: 'full'
  }
];
@NgModule({
  declarations: [
    AppComponent,
    SplashComponent,
    ProfileComponent,
    SantaponecontrolComponent,
    NotificationsComponent,
    NavbarComponent,
    SantaponenavComponent,
    UsernavComponent,
    LoginComponent,
    SignupComponent,
    InformationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CustomMaterialModule,
    FormsModule,
    RouterModule.forRoot(appRoutes,{ enableTracing: true })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
