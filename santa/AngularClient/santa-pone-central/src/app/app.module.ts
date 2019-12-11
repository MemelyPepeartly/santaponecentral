//Necessary
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

//Extras
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CustomMaterialModule } from './core/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

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
import { GeneralInformationComponent } from './splash/general-information/general-information.component';
import { WhatisInformationComponent } from './splash/whatis-information/whatis-information.component';
import { PrivacyInformationComponent } from './splash/privacy-information/privacy-information.component';

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
    GeneralInformationComponent,
    WhatisInformationComponent,
    PrivacyInformationComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CustomMaterialModule,
    FormsModule,
    AppRoutingModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
