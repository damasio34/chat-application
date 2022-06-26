import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './login/login.component';
import { ChatComponent } from './chat/chat.component';
import { LoginGuard } from './login/login.guard';
import { LoginService } from './login/login.service';
import { MaterialModule } from './shared/material/material.module';
import { ChatMessageComponent } from './chat/chat-message/chat-message.component';
import { ChatMessageSenderComponent } from './chat/chat-message-sender/chat-message-sender.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ChatComponent,
    ChatMessageComponent,
    ChatMessageSenderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,

    MaterialModule
  ],
  providers: [ LoginService, LoginGuard ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
