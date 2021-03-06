import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { LoginService } from 'src/app/login/login.service';
import { ChatMessage } from '../chat-message';

@Component({
  selector: 'app-chat-message-sender',
  templateUrl: './chat-message-sender.component.html',
  styleUrls: ['./chat-message-sender.component.scss']
})
export class ChatMessageSenderComponent implements OnInit {

  @ViewChild('textinput')
  textinput!: ElementRef;

  form: FormGroup = new FormGroup({
    username: new FormControl(''),
    room: new FormControl('defaultRoom'),
    text: new FormControl(''),
  });

  text: string = '';
  private username: string | null ='';

  constructor(private fb: FormBuilder, private loginService: LoginService) { }

  @Output() onSendMessage: EventEmitter<ChatMessage> = new EventEmitter<ChatMessage>();

  ngOnInit(): void {
    this.username = this.loginService.username;
    this.createForm(this.username);
  }

  private createForm(username: string | null, defaultRoom?: string): void {
    this.form = this.fb.group({
      username: [username],
      room: [defaultRoom ?? 'defaultRoom'],
      text: ['']
    });
  }

  public sendMessage(): void {
      const message: ChatMessage = this.form.value;
      if (!message.text) return;

      this.onSendMessage.emit(message);
      this.form.reset();
      this.createForm(this.username, message.room);
      this.textinput.nativeElement.focus();
  }

}
