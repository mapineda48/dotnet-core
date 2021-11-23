export default function Root(props: Props) {
  return <>{props.children}</>;
}

/**
 * Interface
 */
interface Props {
  children: React.ReactNode;
}
