﻿import { MemoryPackWriter } from "./memorypack/MemoryPackWriter.js";
import { MemoryPackReader } from "./memorypack/MemoryPackReader.js";
import { AllConvertableType } from "./memorypack/AllConvertableType.js";
import { NoMarkByteEnum } from "./memorypack/NoMarkByteEnum.js";
import { NumberedUShortEnum } from "./memorypack/NumberedUShortEnum.js";
import { NestedObject } from "./memorypack/NestedObject.js";
import { IMogeUnion } from "./memorypack/IMogeUnion.js";
import { SampleUnion1 } from "./memorypack/SampleUnion1.js";
import { Subset } from "./memorypack/Subset.js";
import { SampleUnion2 } from "./memorypack/SampleUnion2.js";
import { Person } from "./memorypack/Person.js";
import { Gender } from "./memorypack/Gender.js";

export async function hoge() {



    let person = new Person();
    person.id = crypto.randomUUID();
    person.age = 30;
    person.firstName = "foo";
    person.lastName = "bar";
    person.dateOfBirth = new Date(1999, 12, 31, 0, 0, 0);
    person.gender = Gender.Other;
    person.emails = ["foo@bar.com", "zoo@bar.net"];

    // serialize to Uint8Array
    let bin = Person.serialize(person);

    let blob = new Blob([bin.buffer], { type: "application/x-memorypack" })

    let response = await fetch("http://localhost:5260/api",
        { method: "POST", body: blob, headers: { "Content-Type": "application/x-memorypack" } });

    let buffer = await response.arrayBuffer();

    // deserialize from ArrayBuffer 
    let person2 = Person.deserialize(buffer);


}

export async function huga() {


    let person1 = new Person();
    person1.id = crypto.randomUUID();
    person1.age = 30;
    person1.firstName = "foo";
    person1.lastName = "bar";
    person1.dateOfBirth = new Date(1999, 12, 31, 0, 0, 0);
    person1.gender = Gender.Other;
    person1.emails = ["foo@bar.com", "zoo@bar.net"];

    let person2 = new Person();
    person2.id = crypto.randomUUID();
    person2.age = 430;
    person2.firstName = "zoo";
    person2.lastName = "zzz";
    person2.dateOfBirth = new Date(2229, 12, 31, 0, 0, 0);
    person2.gender = Gender.Female;
    person2.emails = null;

    let array = Person.serializeArray([person1, person2]);

    try {
        let two = Person.deserializeArray(array.buffer);
        console.log(two);
    }
    catch (e) {
        console.log("no:" + e);
    }

}


export async function test() {

    const date = new Date();

    // setup
    const v = new AllConvertableType();
    v.myBool = true;
    v.myByte = 10;
    v.mySByte = -99;
    v.myShort = -1000;
    v.myInt = 99999;
    v.myLong = 2124141414999n;
    v.myUShort = 43141;
    v.myUInt = 400000;
    v.myULong = 243242n;
    v.myFloat = 31413.431251;
    v.myDouble = 9932.425252;
    v.myGuid = crypto.randomUUID();
    v.myDate = date;
    v.myEnum1 = NoMarkByteEnum.Grape;
    v.myEnum2 = NumberedUShortEnum.Saitama;

    v.myString = "あいうえおかきくけこさしすせそ"; // japanese
    v.myBytes = new Uint8Array([10, 20, 10, 40, 99, 1000]);
    v.myIntArray = [999999, 10, 2131, 412, -42141];
    v.myStringArray = ["hogehgoe", "hugahgua", null, "漢字"];
    v.myList = [10, 20, 30, 40, 50];
    v.myDictionary = new Map<number, number>();
    v.myDictionary.set(100, 99999);
    v.myDictionary.set(200, 40000);
    v.myDictionary.set(150, 50000);
    v.mySet = new Set<number>();
    v.mySet.add(10);
    v.mySet.add(40);
    v.mySet.add(20);
    v.mySet.add(30);

    v.myNestedNested = [
        new Map<number, Set<string[]>>(),
        new Map<number, Set<string[]>>(),
        new Map<number, Set<string[]>>(),
    ]
    const setA = new Set<string[]>();
    setA.add(["hoge", "huga"]);
    setA.add(["a", "びー", "シー"]);
    const setB = new Set<string[]>();
    setA.add(["zako", "notzako"]);
    setA.add(["a", "b", "c", "d", "いー"]);
    const setC = new Set<string[]>();

    v.myNestedNested[0]?.set(100, setA);
    v.myNestedNested[1]?.set(200, setB);
    v.myNestedNested[2]?.set(300, setC);

    v.dictCheck2 = new Map<NoMarkByteEnum, boolean>();
    v.dictCheck2.set(NoMarkByteEnum.Apple, true);
    v.dictCheck2.set(NoMarkByteEnum.Orange, false);
    v.dictCheck3 = new Map<string, number>();
    v.dictCheck3.set(crypto.randomUUID(), 100);
    v.dictCheck3.set(crypto.randomUUID(), null);
    v.dictCheck4X = new Map<number, string>();
    v.dictCheck4X.set(9, "hogemoge");
    v.dictCheck4X.set(19, null);

    v.nested1 = new NestedObject();
    v.nested1.myProperty = 99999;
    v.nested1.myProperty2 = "hogemogeあいううえもげもげもげ";

    const vv = new SampleUnion1();
    vv.myProperty = 9999;
    v.union1 = vv;

    // call

    const bin = AllConvertableType.serialize(v);
    const blob = new Blob([bin.buffer], { type: "application/x-memorypack" })


    const response = await fetch("http://localhost:5260/api/", { method: "POST", body: blob, headers: { "Content-Type": "application/x-memorypack" } });
    if (response.status != 200) {
        console.log(response.status);
        return;
    }
    const buffer = await response.arrayBuffer();

    const v2 = AllConvertableType.deserialize(buffer);

    assertObject(v, v2!);
}

export async function test2() {

    const date = new Date();

    // setup
    const v = new AllConvertableType();
    v.nullMyBool = true;
    v.nullMyByte = 10;
    v.nullMySByte = -99;
    v.nullMyShort = -1000;
    v.nullMyInt = 99999;
    v.nullMyLong = 2124141414999n;
    v.nullMyUShort = 43141;
    v.nullMyUInt = 400000;
    v.nullMyULong = 243242n;
    v.nullMyFloat = 31413.431251;
    v.nullMyDouble = 9932.425252;
    v.nullMyGuid = crypto.randomUUID();
    v.nullMyDate = date;
    v.nullMyEnum1 = NoMarkByteEnum.Grape;
    v.nullMyEnum2 = NumberedUShortEnum.Saitama;

    const vv = new SampleUnion2();
    vv.myProperty = "hogetakoあおえ";
    v.union1 = vv;

    // call
    const bin = AllConvertableType.serialize(v);
    const blob = new Blob([bin.buffer], { type: "application/x-memorypack" })


    const response = await fetch("http://localhost:5260/api/", { method: "POST", body: blob, headers: { "Content-Type": "application/x-memorypack" } });
    if (response.status != 200) {
        console.log(response.status);
        return;
    }
    const buffer = await response.arrayBuffer();

    const v2 = AllConvertableType.deserialize(buffer);

    assertObject(v, v2!);
}

function assertObject(v: AllConvertableType, v2: AllConvertableType): void {
    ok(v.myBool, v2.myBool);
    ok(v.myByte, v2.myByte);
    ok(v.mySByte, v2.mySByte);
    ok(v.myShort, v2.myShort);
    ok(v.myInt, v2.myInt);
    ok(v.myLong, v2.myLong);
    ok(v.myUShort, v2.myUShort);
    ok(v.myUInt, v2.myUInt);
    ok(v.myULong, v2.myULong);
    ok(v.myFloat.toFixed(1), v2.myFloat.toFixed(1));
    ok(v.myDouble.toFixed(1), v2.myDouble.toFixed(1));
    ok(v.myGuid, v2.myGuid);
    ok(v.myDate.getTime(), v2.myDate.getTime());
    ok(v.myEnum1, v2.myEnum1);
    ok(v.myEnum2, v2.myEnum2);

    ok(v.myString, v2.myString);

    if (v.myBytes != null) {
        ok(v.myBytes!.length, v2.myBytes!.length);
        for (let i = 0; i < v.myBytes!.length; i++) {
            ok(v.myBytes![i], v2.myBytes![i]);
        }
    }

    if (v.myIntArray != null) {
        ok(v.myIntArray!.length, v2.myIntArray!.length);
        for (let i = 0; i < v.myIntArray!.length; i++) {
            ok(v.myIntArray![i], v2.myIntArray![i]);
        }
    }

    if (v.myStringArray != null) {
        ok(v.myStringArray!.length, v2.myStringArray!.length);
        for (let i = 0; i < v.myStringArray!.length; i++) {
            ok(v.myStringArray![i], v2.myStringArray![i]);
        }
    }

    if (v.myList != null) {
        ok(v.myList!.length, v2.myList!.length);
        for (let i = 0; i < v.myList!.length; i++) {
            ok(v.myList![i], v2.myList![i]);
        }
    }

    if (v.myDictionary != null) {
        v.myDictionary?.forEach((v, k) => {
            ok(v, v2.myDictionary?.get(k));
        });
    }

    if (v.mySet != null) {
        v.mySet?.forEach(v => {
            ok(true, v2.mySet?.has(v));
        });
    }

    ok(v.nested1?.myProperty, v2.nested1?.myProperty);
    ok(v.nested1?.myProperty2, v2.nested1?.myProperty2);

    if (v.union1 instanceof SampleUnion1 && v2.union1 instanceof SampleUnion1) {
        ok(v.union1.myProperty, v2.union1.myProperty);
    } else if (v.union1 instanceof SampleUnion2 && v2.union1 instanceof SampleUnion2) {
        ok(v.union1.myProperty, v2.union1.myProperty);
    } else {
        ok(true, false);
    }

    console.log("test passed");
}

function ok(v1: any, v2: any): void {
    if (v1 === v2) return;
    throw new Error("Invalid v1:" + v1 + " v2:" + v2);
}

