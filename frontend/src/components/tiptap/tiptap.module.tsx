'use client';

import { Editor } from '@tiptap/core';
import Document from '@tiptap/extension-document';
import Paragraph from '@tiptap/extension-paragraph';
import Text from '@tiptap/extension-text';
import Bold from '@tiptap/extension-bold';
import React from 'react';
import { Box, Button } from '@mui/material';
import Heading from '@tiptap/extension-heading';
import { postArticles } from '@/actions/post.articles';
import Blockquote from '@tiptap/extension-blockquote';
import BlockButtonsComponent from './blockbuttons/blockbuttons.component';
import { BulletList, ListItem } from '@tiptap/extension-list';
import Underline from '@tiptap/extension-underline';
import Strike from '@tiptap/extension-strike';
import Italic from '@tiptap/extension-italic';
import Link from '@tiptap/extension-link';
import Youtube from '@tiptap/extension-youtube';
import Image from '@tiptap/extension-image';

interface TiptapProps {
  content?: string; // JSON em string vindo do BD
}

export default function Tiptap({ content }: TiptapProps) {
  const editorContainerRef = React.useRef<HTMLDivElement>(null);
  const [editor, setEditor] = React.useState<Editor | null>(null);

  const [state, action, isPending] = React.useActionState(postArticles, {
    success: false,
  });
  console.log(state);

  React.useEffect(() => {
    if (!editorContainerRef.current) return;
    let parsedContent = '<p>Escreva algo...</p>';
    try {
      if (content) {
        parsedContent = JSON.parse(content);
      }
    } catch (e) {
      console.error('Erro ao parsear conteúdo do banco', e);
    }

    const instance = new Editor({
      element: editorContainerRef.current,
      extensions: [
        Document,
        Paragraph,
        Text,
        Blockquote,
        Image,
        ListItem,
        Youtube.configure({
          controls: false,
          nocookie: true,
        }),
        Bold,
        BulletList,
        Link.configure({
          openOnClick: false,
          autolink: true,
          defaultProtocol: 'https',
          protocols: ['http', 'https'],
          isAllowedUri: (url, ctx) => {
            try {
              // construct URL
              const parsedUrl = url.includes(':')
                ? new URL(url)
                : new URL(`${ctx.defaultProtocol}://${url}`);

              // use default validation
              if (!ctx.defaultValidate(parsedUrl.href)) {
                return false;
              }

              // disallowed protocols
              const disallowedProtocols = ['ftp', 'file', 'mailto'];
              const protocol = parsedUrl.protocol.replace(':', '');

              if (disallowedProtocols.includes(protocol)) {
                return false;
              }

              // only allow protocols specified in ctx.protocols
              const allowedProtocols = ctx.protocols.map((p) =>
                typeof p === 'string' ? p : p.scheme,
              );

              if (!allowedProtocols.includes(protocol)) {
                return false;
              }

              // disallowed domains
              const disallowedDomains = [
                'example-phishing.com',
                'malicious-site.net',
              ];
              const domain = parsedUrl.hostname;

              if (disallowedDomains.includes(domain)) {
                return false;
              }

              // all checks have passed
              return true;
            } catch {
              return false;
            }
          },
          shouldAutoLink: (url) => {
            try {
              // construct URL
              const parsedUrl = url.includes(':')
                ? new URL(url)
                : new URL(`https://${url}`);

              // only auto-link if the domain is not in the disallowed list
              const disallowedDomains = [
                'example-no-autolink.com',
                'another-no-autolink.com',
              ];
              const domain = parsedUrl.hostname;

              return !disallowedDomains.includes(domain);
            } catch {
              return false;
            }
          },
        }),
        Strike,
        Underline,
        Italic,
        Heading.configure({
          levels: [1, 2, 3],
        }),
      ],
      content: parsedContent,
      autofocus: true,
      editable: true,
      injectCSS: false,
    });

    setEditor(instance);

    return () => {
      instance.destroy();
    };
  }, [content]);

  // ENVIA O FORMDATA PARA POSTAR ARTIGO
  const handleSaveArticle = (formData: FormData) => {
    if (!editor) return;
    const content = JSON.stringify(editor.getJSON());
    console.log(content);
    formData.set('content', JSON.stringify(editor.getJSON()));
    return action(formData);
  };

  return (
    <Box
      sx={{
        background: '#ccc',
        width: '1000px',
        height: '600px',
        display: 'flex',
        p: 2,
      }}
    >
      <form action={handleSaveArticle}>
        <BlockButtonsComponent editor={editor} />
        <div className="editor" ref={editorContainerRef} />
        <Button
          variant="contained"
          type="submit"
          sx={{ mt: 2, alignSelf: 'end', textTransform: 'none' }}
        >
          {isPending ? 'Salvando...' : 'Salvar'}
        </Button>
      </form>
    </Box>
  );
}
