// Necessary
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

// Extras
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CustomMaterialModule } from './core/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

// Componenents
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NavbarComponent } from './navbar/navbar.component';
import { UsernavComponent } from './navbar/usernav/usernav.component';
import { LoginComponent } from './home/login/login.component';
import { WhatisInformationComponent } from './home/whatis-information/whatis-information.component';
import { PrivacyInformationComponent } from './home/privacy-information/privacy-information.component';
import { WhoisInformationComponent } from './home/whois-information/whois-information.component';
import { NologinInformationComponent } from './home/nologin-information/nologin-information.component';
import { FaqInformationComponent } from './home/faq-information/faq-information.component';
import { DefaultnavComponent } from './navbar/defaultnav/defaultnav.component';
import { ContactComponent } from './contact/contact.component';
import { SignupComponent } from './signup/signup.component';
import { AdminnavComponent } from './navbar/adminnav/adminnav.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProfileComponent,
    NotificationsComponent,
    NavbarComponent,
    UsernavComponent,
    LoginComponent,
    WhatisInformationComponent,
    PrivacyInformationComponent,
    WhoisInformationComponent,
    NologinInformationComponent,
    FaqInformationComponent,
    DefaultnavComponent,
    ContactComponent,
    SignupComponent,
    AdminnavComponent
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
