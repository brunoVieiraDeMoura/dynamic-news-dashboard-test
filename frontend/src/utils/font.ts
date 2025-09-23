import { Montserrat, Open_Sans } from 'next/font/google';

export const type_second = Montserrat({
  weight: ['400', '500', '600'],
  subsets: ['latin'],
  variable: '--type-first-montserrat',
  display: 'swap',
});

export const type_first = Open_Sans({
  weight: ['400', '500'],
  subsets: ['latin'],
  variable: '--type-second-open-sans',
  display: 'swap',
});
