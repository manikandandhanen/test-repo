import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
//import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentUrlService {
  public authApiURI: string = environment.authApiURI;
  constructor() { }
}