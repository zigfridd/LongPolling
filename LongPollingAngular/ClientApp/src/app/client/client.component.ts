import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import * as signalR from '@microsoft/signalr';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['client.component.css'],
})
export class ClientComponent{
  public messages: string[] = [];
  public isConnected: boolean = false;
  private _hubConnection: HubConnection;
  @Input() clientNumber: number = 0;

  constructor(private _httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string, 
  private readonly simpleService: MessageService) { 
     this._hubConnection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Error)
      .withUrl("http://localhost:5078/messageHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
      this._hubConnection.on('ReceiveMessage', (message) => {
        //console.log(message);
        this.messages.push(message);
        this.simpleService.changeCount({count: this.messages.length, name: this.clientNumber});
      });
      this._hubConnection.start()
      .then(() => {console.log('connection started'); this.isConnected = true;})
      .catch((err) => console.log('error while establishing signalr connection: ' + err));
      let i = 0;
  }

  disconnect(){
    this._hubConnection.stop();
    this.isConnected = false;
  }

  connect(){
    this._hubConnection.start();
    this.isConnected = true;
  }
}

