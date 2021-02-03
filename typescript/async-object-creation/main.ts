/**
 * Asynchronous object creation tool
 * The base AsyncObject can be extended to handle asynchronous creation
 * without repeating code and while preserving code completion for the constructor
 */

class Utilities {
    /**
     * Waits for [delay] and resolves promise with a new value
     * @param delay 
     */
    static async fetchWithDelay(delay: number): Promise<string> {
        // use to simulate delay in fetching data

        let value = 'fetched';

        // return Promise.reject(new Error("can't create timer"));

        return new Promise<string>((resolve) => {
            setTimeout(() => {
                resolve(value);
            }, delay);
        });

    }
}

abstract class AsyncObject {
    // extend to gain asynchronous creation capability
    // the child class is constructed in the asyncConstructor method 

    // used to store the result of the child's asyncConstructor promise (an optional utility output)
    protected constructorOutput: any;

    // set parameters in child constructor
    constructor() {
        // initialize properties in child constructor
    }

    // override and construct object here
    // this returns a promise with a generic result defined in the child class, NOT the instance
    abstract asyncConstructor(...args: any[]): Promise<any>

    // instantiate the object and return it in a promise
    static async createAsync<P extends any[], T extends AsyncObject>(this: new (...args: P) => T, ...args: P) {
        // use: [Child].createAsync(args)
        // the arguments are the same as those provided in the child constructor

        // create object instance
        let instance = new this(...args);

        // call asyncConstructor on instance and wrap it in a promise
        let promise = instance.asyncConstructor(args).then((result) => {
            // store constructor output in new instance
            instance.constructorOutput = result;
            console.log("constructor output: " + result);

            return instance;
        }).catch((err) => {
            console.error(err.message);
            throw new Error("async constructor failed");
        });

        // return the promised instance
        return promise;
    }
}

// test
class TestAsyncChild extends AsyncObject {
    public classProperty = "original";
    public fetchedProperty = "default"; // to simulate a delay in fetching data at construction

    // ask for parameters 
    constructor(param: string) {
        super();
        console.log('default class property value: ' + this.classProperty);
        console.log('default fetched property value instance: ' + this.fetchedProperty);

        // initialize properties in constructor
        this.classProperty = param;
    }

    // build asynchronous constructor
    public async asyncConstructor(): Promise<any> {
        // simulate fetching data with a delay
        let fetchResult = Utilities.fetchWithDelay(2000).then((result) => {
            this.fetchedProperty = result;

            // optionally return a success flag for the construction (or anything else)
            return "success";
        }).catch((err) => {
            console.error(err.message);
            throw new Error("coulnd't fetch result");
        });

        return fetchResult;
    }
}

// create object and manipulate it once instantiation is done
// note that parameters completion is maintained for the child class in the createAsync method
let asyncObject = TestAsyncChild.createAsync("modified");
asyncObject.then((result) => {
    console.log("class property value: " + result.classProperty);
    console.log("fetched property value: " + result.fetchedProperty);
}).catch((err) => {
    console.error(err.message);
    throw new Error("couldn't initialize object");
});
