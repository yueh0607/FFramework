import { MemoryPackWriter } from "./MemoryPackWriter.js";
import { MemoryPackReader } from "./MemoryPackReader.js";

export class SampleLarge {
    _id: string | null;
    author: string | null;
    created_at: string | null;
    description: string | null;
    image: string | null;
    keywords: (string | null)[] | null;
    language: string | null;
    permalink: string | null;
    published: boolean;
    title: string | null;
    updated_at: string | null;
    url: string | null;

    constructor() {
        this._id = null;
        this.author = null;
        this.created_at = null;
        this.description = null;
        this.image = null;
        this.keywords = null;
        this.language = null;
        this.permalink = null;
        this.published = false;
        this.title = null;
        this.updated_at = null;
        this.url = null;

    }

    static serialize(value: SampleLarge | null): Uint8Array {
        const writer = MemoryPackWriter.getSharedInstance();
        this.serializeCore(writer, value);
        return writer.toArray();
    }

    static serializeCore(writer: MemoryPackWriter, value: SampleLarge | null): void {
        if (value == null) {
            writer.writeNullObjectHeader();
            return;
        }

        writer.writeObjectHeader(12);
        writer.writeString(value._id);
        writer.writeString(value.author);
        writer.writeString(value.created_at);
        writer.writeString(value.description);
        writer.writeString(value.image);
        writer.writeArray(value.keywords, (writer, x) => writer.writeString(x));
        writer.writeString(value.language);
        writer.writeString(value.permalink);
        writer.writeBoolean(value.published);
        writer.writeString(value.title);
        writer.writeString(value.updated_at);
        writer.writeString(value.url);

    }

    static serializeArray(value: (SampleLarge | null)[] | null): Uint8Array {
        const writer = MemoryPackWriter.getSharedInstance();
        this.serializeArrayCore(writer, value);
        return writer.toArray();
    }

    static serializeArrayCore(writer: MemoryPackWriter, value: (SampleLarge | null)[] | null): void {
        writer.writeArray(value, (writer, x) => SampleLarge.serializeCore(writer, x));
    }

    static deserialize(buffer: ArrayBuffer): SampleLarge | null {
        return this.deserializeCore(new MemoryPackReader(buffer));
    }

    static deserializeCore(reader: MemoryPackReader): SampleLarge | null {
        const [ok, count] = reader.tryReadObjectHeader();
        if (!ok) {
            return null;
        }

        const value = new SampleLarge();
        if (count == 12) {
            value._id = reader.readString();
            value.author = reader.readString();
            value.created_at = reader.readString();
            value.description = reader.readString();
            value.image = reader.readString();
            value.keywords = reader.readArray(reader => reader.readString());
            value.language = reader.readString();
            value.permalink = reader.readString();
            value.published = reader.readBoolean();
            value.title = reader.readString();
            value.updated_at = reader.readString();
            value.url = reader.readString();

        }
        else if (count > 12) {
            throw new Error("Current object's property count is larger than type schema, can't deserialize about versioning.");
        }
        else {
            if (count == 0) return value;
            value._id = reader.readString(); if (count == 1) return value;
            value.author = reader.readString(); if (count == 2) return value;
            value.created_at = reader.readString(); if (count == 3) return value;
            value.description = reader.readString(); if (count == 4) return value;
            value.image = reader.readString(); if (count == 5) return value;
            value.keywords = reader.readArray(reader => reader.readString()); if (count == 6) return value;
            value.language = reader.readString(); if (count == 7) return value;
            value.permalink = reader.readString(); if (count == 8) return value;
            value.published = reader.readBoolean(); if (count == 9) return value;
            value.title = reader.readString(); if (count == 10) return value;
            value.updated_at = reader.readString(); if (count == 11) return value;
            value.url = reader.readString(); if (count == 12) return value;

        }
        return value;
    }

    static deserializeArray(buffer: ArrayBuffer): (SampleLarge | null)[] | null {
        return this.deserializeArrayCore(new MemoryPackReader(buffer));
    }

    static deserializeArrayCore(reader: MemoryPackReader): (SampleLarge | null)[] | null {
        return reader.readArray(reader => SampleLarge.deserializeCore(reader));
    }
}
