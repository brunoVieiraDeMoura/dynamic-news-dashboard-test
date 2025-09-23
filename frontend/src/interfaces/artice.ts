export interface Mark {
  type: string;
  attrs?: Attrs;
}

export type NodeType =
  | 'doc'
  | 'paragraph'
  | 'heading'
  | 'blockquote'
  | 'bulletList'
  | 'listItem'
  | 'image'
  | 'youtube'
  | 'text';

export interface Node {
  type: NodeType;
  attrs?: Attrs;
  content?: Node[];
  text?: string;
  marks?: Mark[];
}

export interface Doc {
  type: 'doc';
  content: Node[];
}

export interface Message {
  id: number;
  text: string;
  userId: number;
  createdAt: Date;
}

export interface Article {
  id: number;
  categoryId: number;
  writerID: number | null;
  reviewID: number | null;
  title: string;
  slug: string;
  article: Doc;
  data: Date;
  comments: Message[];
}

export interface Attrs {
  href?: string;
  target?: string;
  rel?: string;
  class?: null;
  level?: number;
  src?: string;
  start?: number;
  width?: null | number;
  height?: null | number;
  alt?: string;
  title?: string;
}
