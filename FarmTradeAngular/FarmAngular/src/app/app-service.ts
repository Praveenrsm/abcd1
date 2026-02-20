import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppEntity } from './app-entity';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private Url="http://localhost:5274/api/Account/GetUsers?pageNumber=1&pageSize=10"
  constructor(private Http: HttpClient) {
   }
  getAllUsers():Observable<AppEntity[]> {
    return this.Http.get<AppEntity[]>(this.Url);
  }
}