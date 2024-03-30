import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public count$ = new Subject<any>();

  public changeCount(data: any) {
     this.count$.next(data); 
  }

  constructor() { }
}
