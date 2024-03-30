import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['chat.component.css']
})
export class ChatComponent{
    @Input() items: string[] = [];
}

