import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { map } from 'rxjs/operators';
import { ApiResponse } from '../_models';

@Injectable({ providedIn: 'root' })
export class Proxy
{

  protected constructor(private http: HttpClient) { }

  public call<T>(controller: string, action: string, data: any): Observable<T>
  {
    const url = `${window.location.origin}/${controller}/${action}`;

    return this.http.post<any>(url, data)
      .pipe(map(response =>
      {
        const apiResponse: ApiResponse<T> = response;

          if (!apiResponse.hasError)
            return apiResponse.result;

          throw new Error(apiResponse.message);
      }))
  }


  public callGet<T>(controller: string, action: string): Observable<T>
  {
    const url = `${window.location.origin}/${controller}/${action}`;

    return this.http.get<any>(url)
      .pipe(map(response =>
      {
        const apiResponse: ApiResponse<T> = response;

          if (!apiResponse.hasError)
            return apiResponse.result;

          throw new Error(apiResponse.message);
      }))
  }
}
