import { Component, Input } from '@angular/core';
import { ChatMessage } from '../chat-message';

@Component({
  selector: 'app-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.scss']
})
export class ChatMessageComponent implements ChatMessage {

  @Input() id!: number;
  @Input() username!: string;
  @Input() text!: string;
  @Input() room!: string;

  constructor() { }

}
