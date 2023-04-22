import { Injectable } from '@angular/core';
import { Subject, Subscription } from 'rxjs';

export interface IEventListener {
    unsubscribe(): void;
}

interface IBrokeredEventBase {
    name: string;
    emit(data: any): void;
    listen(next: (data: any) => void): IEventListener;
}

interface IBrokeredEvent<T> extends IBrokeredEventBase {
    emit(data: any): void;
    listen(next: (data: any) => void): IEventListener;
}

class EventListener implements IEventListener {
    constructor(private subscription: Subscription) {
    }

    public unsubscribe(): void {
        if (this.subscription)
            this.subscription.unsubscribe();
    }
}

class BrokeredEvent<T> implements IBrokeredEvent<T> {
    private subject: Subject<T>;

    constructor(public name: string) {
        this.subject = new Subject<T>();
    }

    public emit(data: T): void {
        this.subject.next(data);
    }

    public listen(next: (value: T) => void): IEventListener {
        return new EventListener(this.subject.subscribe(next));
    }
}

@Injectable()
export class EventBrokerService {
    private events: { [name: string]: IBrokeredEventBase };

    constructor() {
        this.events = {};
    }

    public register<T>(eventName: string): BrokeredEvent<T> {
        //console.log('Recieved event named: ', eventName);

        var event = this.events[eventName];
        if (typeof event === 'undefined') {
            event = this.events[eventName] = new BrokeredEvent<T>(eventName);
        }
        return event as BrokeredEvent<T>;
    }

    public listen<T>(eventName: string, next: (value: T) => void): IEventListener {
        //console.log('Listening... event named: ', eventName);
        return this.register<T>(eventName).listen(next);
    }

    public emit<T>(eventName: string, data: T): void {
        //console.log('Emitted in event broker event named: ', eventName);
        return this.register<T>(eventName).emit(data);
    }
}
