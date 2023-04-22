import { Injectable, PipeTransform } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Proxy } from '../_helpers';
import { Room, User } from '../_models';
import { SortColumn, SortDirection } from '../_helpers/sortable.directive';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { Registration } from '../_models/registration';
import { map } from 'rxjs/operators';

interface SearchResult
{
  users: User[];
  total: number;
}

interface State
{
  page: number;
  pageSize: number;
  searchTerm: string;
  sortColumn: SortColumn;
  sortDirection: SortDirection;
}


@Injectable({ providedIn: 'root' })
export class RegistrationService
{

  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _users$ = new BehaviorSubject<User[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private _users: User[]=[];

  private _state: State = {
    page: 1,
    pageSize: 4,
    searchTerm: '',
    sortColumn: '',
    sortDirection: ''
  };


  constructor(private http: HttpClient, private proxy: Proxy)
  { 
  //  this.getAll().subscribe((r:User[]) =>
  //  {
  //    this._users = r;
  //    this._search$.next();
  //  });

  }

  getAll(): Observable<Registration[]>
  {
    return this.proxy.call<Registration[]>("registration", "getAll", {});
  }
  save(registration: Registration): Observable<User>
  {
    return this.proxy.call<User>("registration", "save", registration);
  }
  export(): Observable<any> {
    const url = `${window.location.origin}/registration/export`;

    return this.http.post(url, {}, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/octet-stream")
    });
  }
  exportRnd(nRow: number, totalRow: number): Observable<any> {
    const url = `${window.location.origin}/registration/exportRnd`;

    return this.http.post(url, { NRow: nRow, TotalRow: totalRow }, {
      responseType: "blob",
      headers: new HttpHeaders().append("Content-Type", "application/json")
    });
  }
  addRoom(user: User, room: Room): Observable<boolean>
  {
    return this.proxy.call<boolean>("user", "addRoom", { userId: user.id, roomId: room.id });
  }
  deleteRoom(user: User, room: Room): Observable<boolean>
  {
    return this.proxy.call<boolean>("user", "deleteRoom", { userId: user.id, roomId: room.id });
  }

  get users$() { return this._users$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }

  set page(page: number) { this._set({ page }); }
  set pageSize(pageSize: number) { this._set({ pageSize }); }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }

  private _set(patch: Partial<State>)
  {
    Object.assign(this._state, patch);
    this._search$.next();
  }

}
